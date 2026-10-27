
import { Service, inject, signal } from '@angular/core';
import { Observable, map, switchMap, tap, throwError } from 'rxjs';

import { Categoria, Platillo } from '../models/platillo.model';
import { Mesa } from '../models/mesa.model';
import { DetallePedido, Pedido } from '../models/pedido.model';
import { RestauranteApiService } from './restaurante-api.service';

import {
  ClienteApi,
  MesaApi,
  MesaRequest,
  PedidoRequest,
  PedidoResponse
} from '../models/api.model';

@Service()
export class RestauranteService {
  private readonly api = inject(RestauranteApiService);

  private readonly _platillos = signal<Platillo[]>([]);
  private readonly _categorias = signal<Categoria[]>([]);
  private readonly _mesas = signal<Mesa[]>([]);
  private readonly _pedidos = signal<Pedido[]>([]);

  readonly platillos = this._platillos.asReadonly();
  readonly categorias = this._categorias.asReadonly();
  readonly mesas = this._mesas.asReadonly();
  readonly pedidos = this._pedidos.asReadonly();

  constructor() {
    this.cargarPlatillos().subscribe({
      error: (error) =>
        console.error('Error al cargar productos:', error)
    });

    this.cargarCategorias();
    this.cargarMesas();
    this.cargarPedidos();
  }

  cargarPlatillos(): Observable<void> {
    return this.api.obtenerProductos().pipe(
      map((productos) => {
        this._platillos.set(
          productos.map((producto) => ({
            id: producto.id,
            nombre: producto.nombre,
            descripcion: producto.descripcion ?? '',
            precio: producto.precio,
            categoriaId: producto.categoriaId,
            disponible: producto.disponible,
            imagenUrl: producto.imagenUrl
          }))
        );
      })
    );
  }

  cargarCategorias(): void {
    this.api.obtenerCategorias().subscribe({
      next: (categorias) => {
        this._categorias.set(
          categorias.map((categoria) => ({
            id: categoria.id,
            nombre: categoria.nombre
          }))
        );
      },
      error: (error) =>
        console.error('Error al cargar categorías:', error)
    });
  }

  cargarMesas(): void {
    this.api.obtenerMesas().subscribe({
      next: (mesas: MesaApi[]) => {
        this._mesas.set(
          mesas.map((mesa) => ({
            id: mesa.id,
            numero: mesa.numero,
            capacidad: mesa.capacidad,
            estado: mesa.estado as Mesa['estado']
          }))
        );
      },
      error: (error) =>
        console.error('Error al cargar mesas:', error)
    });
  }

  crearMesa(mesa: MesaRequest): Observable<MesaApi> {
    return this.api.crearMesa(mesa).pipe(
      tap((mesaCreada) => {
        if (mesaCreada) {
          this._mesas.update((actuales) => [
            ...actuales,
            {
              id: mesaCreada.id,
              numero: mesaCreada.numero,
              capacidad: mesaCreada.capacidad,
              estado: mesaCreada.estado as Mesa['estado']
            }
          ]);
        }
      })
    );
  }

  cargarPedidos(): void {
    this.api.obtenerPedidos().subscribe({
      next: (pedidos) => {
        this._pedidos.set(
          pedidos.map((pedido) => this.adaptarPedido(pedido))
        );
      },
      error: (error) =>
        console.error('Error al cargar pedidos:', error)
    });
  }

  agregarPedido(
    mesaId: number,
    clienteNombre: string,
    detalles: DetallePedido[]
  ): Observable<Pedido> {
    if (detalles.length === 0) {
      return throwError(
        () => new Error('Agrega al menos un producto al pedido.')
      );
    }

    const nombreBuscado = clienteNombre.trim().toLocaleLowerCase();

    if (!nombreBuscado) {
      return throwError(
        () => new Error('Selecciona un cliente válido.')
      );
    }

    return this.api.obtenerClientes().pipe(
      switchMap((clientes: ClienteApi[]) => {
        const cliente = clientes.find(
          (c) =>
            `${c.nombre} ${c.apellido}`
              .trim()
              .toLocaleLowerCase() === nombreBuscado
        );

        if (!cliente) {
          return throwError(
            () => new Error('El cliente no está registrado en el backend.')
          );
        }

        const solicitud: PedidoRequest = {
          mesaId,
          clienteId: cliente.id,
          detalles: detalles.map((detalle) => ({
            productoId: detalle.platilloId,
            cantidad: detalle.cantidad
          }))
        };

        return this.api.crearPedido(solicitud);
      }),
      map((respuesta: PedidoResponse) => {
        if (!respuesta) {
          throw new Error('El backend no confirmó el registro del pedido.');
        }

        return this.adaptarPedido(respuesta);
      }),
      tap((pedido) => {
        this._pedidos.update((actuales) => [
          ...actuales,
          pedido
        ]);
      })
    );
  }

  private adaptarPedido(respuesta: PedidoResponse): Pedido {
    return {
      id: respuesta.id,
      mesaId: respuesta.mesaId,
      clienteNombre: respuesta.nombreCliente,
      fecha: respuesta.fecha,
      estado: respuesta.estado as Pedido['estado'],
      detalles: respuesta.detalles.map((detalle) => ({
        platilloId: detalle.productoId,
        nombrePlatillo: detalle.nombreProducto,
        precioUnitario: detalle.precioUnitario,
        cantidad: detalle.cantidad
      }))
    };
  }

  totalPedido(pedido: Pedido): number {
    return pedido.detalles.reduce(
      (total, detalle) =>
        total + detalle.precioUnitario * detalle.cantidad,
      0
    );
  }
}
