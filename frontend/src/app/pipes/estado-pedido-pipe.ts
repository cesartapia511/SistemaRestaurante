import { Pipe, PipeTransform } from '@angular/core';
import { EstadoPedido } from '../models/pedido.model';

@Pipe({ name: 'estadoPedido' })
export class EstadoPedidoPipe implements PipeTransform {
  transform(estado: EstadoPedido): string {
    const map: Record<EstadoPedido, string> = {
      pendiente: 'Pendiente',
      preparando: 'Preparando',
      servido: 'Servido',
      pagado: 'Pagado',
      cancelado: 'Cancelado',
    };
    return map[estado] ?? estado;
  }
}