export type EstadoPedido = 'pendiente' | 'preparando' | 'servido' | 'pagado' | 'cancelado';

export interface DetallePedido {
  platilloId: number;
  nombrePlatillo: string;
  precioUnitario: number;
  cantidad: number;
}

export interface Pedido {
  id: number;
  mesaId: number;
  clienteNombre: string;
  fecha: string;
  detalles: DetallePedido[];
  estado: EstadoPedido;
}