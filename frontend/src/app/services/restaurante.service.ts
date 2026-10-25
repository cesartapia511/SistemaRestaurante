import { Service, signal } from '@angular/core';
import { Categoria, Platillo } from '../models/platillo.model';
import { Mesa } from '../models/mesa.model';
import { DetallePedido, Pedido } from '../models/pedido.model';

@Service()
export class RestauranteService {
  private readonly _platillos = signal<Platillo[]>([
    { id: 1, nombre: 'Silpancho',       descripcion: 'Carne, arroz, papa, huevo', precio: 25, categoriaId: 1, disponible: true },
    { id: 2, nombre: 'Pique Macho',     descripcion: 'Carne, papas, locoto',       precio: 45, categoriaId: 1, disponible: true },
    { id: 3, nombre: 'Sopa de maní',    descripcion: 'Sopa tradicional',           precio: 15, categoriaId: 2, disponible: true },
    { id: 4, nombre: 'Jugo de naranja', descripcion: 'Natural',                    precio: 8,  categoriaId: 3, disponible: true },
  ]);

  private readonly _categorias = signal<Categoria[]>([
    { id: 1, nombre: 'Platos fuertes' },
    { id: 2, nombre: 'Sopas' },
    { id: 3, nombre: 'Bebidas' },
  ]);

  private readonly _mesas = signal<Mesa[]>([
    { id: 1, numero: 1, capacidad: 4, estado: 'libre' },
    { id: 2, numero: 2, capacidad: 2, estado: 'ocupada' },
    { id: 3, numero: 3, capacidad: 6, estado: 'reservada' },
  ]);

  private readonly _pedidos = signal<Pedido[]>([]);

  readonly platillos = this._platillos.asReadonly();
  readonly categorias = this._categorias.asReadonly();
  readonly mesas = this._mesas.asReadonly();
  readonly pedidos = this._pedidos.asReadonly();

  agregarPedido(mesaId: number, clienteNombre: string, detalles: DetallePedido[]): Pedido {
    const nuevo: Pedido = {
      id: Date.now(),
      mesaId,
      clienteNombre,
      fecha: new Date().toISOString(),
      detalles,
      estado: 'pendiente',
    };
    this._pedidos.update((prev) => [...prev, nuevo]);
    return nuevo;
  }

  totalPedido(p: Pedido): number {
    return p.detalles.reduce((acc, d) => acc + d.precioUnitario * d.cantidad, 0);
  }
}