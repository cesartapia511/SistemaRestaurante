
import { Component, computed, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';

import { RestauranteService } from '../../services/restaurante.service';
import { RestauranteApiService } from '../../services/restaurante-api.service';
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
  private readonly apiHttp = inject(RestauranteApiService);

  readonly platillos = this.api.platillos;
  readonly mesas = this.api.mesas;
  readonly pedidos = this.api.pedidos;

  readonly mesaSeleccionada = signal<number | null>(null);
  readonly clienteNombre = signal('');
  readonly lineas = signal<DetallePedido[]>([]);
  readonly error = signal<string | null>(null);

  readonly nuevoClienteNombre = signal('');
  readonly nuevoClienteApellido = signal('');
  readonly nuevoClienteTelefono = signal('');
  readonly guardandoCliente = signal(false);
  readonly mensajeCliente = signal('');
  readonly errorCliente = signal('');

  readonly guardandoPedido = signal(false);
  readonly mensajePedido = signal('');

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

  onNuevoClienteNombre(event: Event): void {
    this.nuevoClienteNombre.set(
      (event.target as HTMLInputElement).value
    );
  }

  onNuevoClienteApellido(event: Event): void {
    this.nuevoClienteApellido.set(
      (event.target as HTMLInputElement).value
    );
  }

  onNuevoClienteTelefono(event: Event): void {
    this.nuevoClienteTelefono.set(
      (event.target as HTMLInputElement).value
    );
  }

  registrarCliente(): void {
    if (this.guardandoCliente()) {
      return;
    }

    this.mensajeCliente.set('');
    this.errorCliente.set('');

    const nombre = this.nuevoClienteNombre().trim();
    const apellido = this.nuevoClienteApellido().trim();
    const telefono = this.nuevoClienteTelefono().trim();

    if (!nombre || nombre.length > 50) {
      this.errorCliente.set(
        'El nombre es obligatorio y debe tener máximo 50 caracteres.'
      );
      return;
    }

    if (!apellido || apellido.length > 50) {
      this.errorCliente.set(
        'El apellido es obligatorio y debe tener máximo 50 caracteres.'
      );
      return;
    }

    this.guardandoCliente.set(true);

    this.apiHttp.crearCliente({
      nombre,
      apellido,
      telefono: telefono || null
    })
      .pipe(
        finalize(() => this.guardandoCliente.set(false))
      )
      .subscribe({
        next: (cliente) => {
          if (!cliente) {
            this.errorCliente.set(
              'No se pudo registrar el cliente.'
            );
            return;
          }

          this.clienteNombre.set(
            `${cliente.nombre} ${cliente.apellido}`
          );

          this.mensajeCliente.set(
            `Cliente ${cliente.nombre} ${cliente.apellido} registrado correctamente.`
          );

          this.nuevoClienteNombre.set('');
          this.nuevoClienteApellido.set('');
          this.nuevoClienteTelefono.set('');
        },
        error: (error) => {
          console.error('Error al registrar cliente:', error);

          this.errorCliente.set(
            'No se pudo registrar el cliente. Revisa la conexión con el backend.'
          );
        }
      });
  }

  agregarLinea(platilloId: number): void {
    if (this.guardandoPedido()) {
      return;
    }

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
    if (this.guardandoPedido()) {
      return;
    }

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
    if (this.guardandoPedido()) {
      return;
    }

    const mesa = this.mesaSeleccionada();
    const cliente = this.clienteNombre().trim();
    const detalles = this.lineas();

    this.error.set(null);
    this.mensajePedido.set('');

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

    this.guardandoPedido.set(true);

    this.api.agregarPedido(mesa, cliente, detalles)
      .pipe(
        finalize(() => this.guardandoPedido.set(false))
      )
      .subscribe({
        next: (pedido) => {
          this.mensajePedido.set(
            `Pedido ${pedido.id} registrado correctamente.`
          );

          this.lineas.set([]);
          this.mesaSeleccionada.set(null);
          this.clienteNombre.set('');
        },
        error: (error) => {
          console.error('Error al registrar pedido:', error);

          this.error.set(
            error instanceof Error && !('status' in error)
              ? error.message
              : 'No se pudo registrar el pedido. Revisa la conexión con el backend.'
          );
        }
      });
  }
}
