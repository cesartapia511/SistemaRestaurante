
import { Component, computed, inject, signal } from '@angular/core';

import { RestauranteService } from '../../services/restaurante.service';
import { EstadoMesaDirective } from '../../directives/estado-mesa';
import { ResaltadoDirective } from '../../directives/resaltado';
import { MonedaBsPipe } from '../../pipes/moneda-bs-pipe';
import { EstadoPedidoPipe } from '../../pipes/estado-pedido-pipe';
import { DetallePedido, Pedido } from '../../models/pedido.model';

@Component({
  imports: [
    EstadoMesaDirective,
    ResaltadoDirective,
    MonedaBsPipe,
    EstadoPedidoPipe
  ],
  selector: 'app-pedidos',
  styleUrl: './pedidos.css',
  templateUrl: './pedidos.html'
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
    this.lineas().reduce(
      (acc, linea) =>
        acc + linea.precioUnitario * linea.cantidad,
      0
    )
  );

  onCliente(event: Event): void {
    this.clienteNombre.set(
      (event.target as HTMLInputElement).value
    );
  }

  agregarLinea(platilloId: number): void {
    const platillo = this.api.platillos().find(
      (item) => item.id === platilloId
    );

    if (!platillo) {
      return;
    }

    this.lineas.update((actuales) => {
      const indice = actuales.findIndex(
        (linea) => linea.platilloId === platilloId
      );

      if (indice >= 0) {
        const copia = [...actuales];

        copia[indice] = {
          ...copia[indice],
          cantidad: copia[indice].cantidad + 1
        };

        return copia;
      }

      return [
        ...actuales,
        {
          platilloId,
          nombrePlatillo: platillo.nombre,
          precioUnitario: platillo.precio,
          cantidad: 1
        }
      ];
    });
  }

  quitarLinea(platilloId: number): void {
    this.lineas.update((actuales) =>
      actuales.filter(
        (linea) => linea.platilloId !== platilloId
      )
    );
  }

  totalDePedido(pedido: Pedido): number {
    return this.api.totalPedido(pedido);
  }

  guardarPedido(): void {
    const mesa = this.mesaSeleccionada();
    const cliente = this.clienteNombre().trim();
    const detalles = this.lineas();

    if (!cliente) {
      this.error.set('Escribe el nombre del cliente.');
      return;
    }

    if (mesa === null) {
      this.error.set('Selecciona una mesa.');
      return;
    }

    if (detalles.length === 0) {
      this.error.set('Agrega al menos un platillo.');
      return;
    }

    this.error.set(null);

    this.api.agregarPedido(mesa, cliente, detalles);

    this.lineas.set([]);
    this.mesaSeleccionada.set(null);
    this.clienteNombre.set('');
  }
}
