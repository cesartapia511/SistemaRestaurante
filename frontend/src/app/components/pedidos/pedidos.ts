import { Component, computed, inject, signal } from '@angular/core';
import { RestauranteService } from '../../services/restaurante.service';
import { EstadoMesaDirective } from '../../directives/estado-mesa';
import { ResaltadoDirective } from '../../directives/resaltado';
import { MonedaBsPipe } from '../../pipes/moneda-bs-pipe';
import { EstadoPedidoPipe } from '../../pipes/estado-pedido-pipe';
import { DetallePedido, Pedido } from '../../models/pedido.model';

@Component({
  imports: [EstadoMesaDirective, ResaltadoDirective, MonedaBsPipe, EstadoPedidoPipe],
  selector: 'app-pedidos',
  styleUrl: './pedidos.css',
  templateUrl: './pedidos.html',
})
export class Pedidos {
  private readonly api = inject(RestauranteService);

  readonly platillos = this.api.platillos;
  readonly mesas = this.api.mesas;
  readonly pedidos = this.api.pedidos;

  readonly mesaSeleccionada = signal<number | null>(null);
  readonly clienteNombre = signal('');
  readonly lineas = signal<DetallePedido[]>([]);
  readonly error = signal<string | null>(null);

  readonly total = computed(() =>
    this.lineas().reduce((acc, l) => acc + l.precioUnitario * l.cantidad, 0)
  );

  onCliente(e: Event) {
    this.clienteNombre.set((e.target as HTMLInputElement).value);
  }

  agregarLinea(platilloId: number) {
    const p = this.api.platillos().find((x) => x.id === platilloId);
    if (!p) return;
    this.lineas.update((prev) => {
      const idx = prev.findIndex((l) => l.platilloId === platilloId);
      if (idx >= 0) {
        const copia = [...prev];
        copia[idx] = { ...copia[idx], cantidad: copia[idx].cantidad + 1 };
        return copia;
      }
      return [...prev, {
        platilloId,
        nombrePlatillo: p.nombre,
        precioUnitario: p.precio,
        cantidad: 1,
      }];
    });
  }

  quitarLinea(platilloId: number) {
    this.lineas.update((prev) => prev.filter((l) => l.platilloId !== platilloId));
  }

  totalDePedido(p: Pedido): number {
    return this.api.totalPedido(p);
  }

  guardarPedido() {
    const mesa = this.mesaSeleccionada();

    if (!this.clienteNombre().trim()) {
      this.error.set('Escribe el nombre del cliente.');
      return;
    }
    if (mesa === null) {
      this.error.set('Selecciona una mesa.');
      return;
    }
    if (this.lineas().length === 0) {
      this.error.set('Agrega al menos un platillo.');
      return;
    }

    this.error.set(null);
    this.api.agregarPedido(mesa, this.clienteNombre().trim(), this.lineas());
    this.lineas.set([]);
    this.mesaSeleccionada.set(null);
    this.clienteNombre.set('');
  }
}
