
export interface ProductoApi {
  id: number;
  nombre: string;
  descripcion: string | null;
  imagenUrl: string | null;
  precio: number;
  disponible: boolean;
  categoriaId: number;
  nombreCategoria: string;
}

export interface CategoriaApi {
  id: number;
  nombre: string;
  descripcion: string | null;
  estado: boolean;
}

export interface MesaApi {
  id: number;
  numero: number;
  capacidad: number;
  estado: string;
}

export interface ClienteApi {
  id: number;
  nombre: string;
  apellido: string;
  telefono: string | null;
}

export interface DetallePedidoRequest {
  productoId: number;
  cantidad: number;
}

export interface PedidoRequest {
  mesaId: number;
  clienteId: number;
  metodoPago?: string | null;
  detalles: DetallePedidoRequest[];
}

export interface DetallePedidoResponse {
  id: number;
  productoId: number;
  nombreProducto: string;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
}

export interface PedidoResponse {
  id: number;
  fecha: string;
  estado: string;
  total: number;
  metodoPago: string | null;
  mesaId: number;
  numeroMesa: number;
  clienteId: number;
  nombreCliente: string;
  detalles: DetallePedidoResponse[];
}
export interface MesaRequest {
  numero: number;
  capacidad: number;
}
export interface ClienteRequest {
  nombre: string;
  apellido: string;
  telefono: string | null;
}
export interface ProductoRequest {
  nombre: string;
  descripcion: string | null;
  imagenUrl: string | null;
  precio: number;
  categoriaId: number;
}
