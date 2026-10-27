
import { HttpClient } from '@angular/common/http';
import { Service, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ProductoApi,
  CategoriaApi,
  MesaApi,
  MesaRequest,
  ClienteApi,
  ClienteRequest,
  PedidoRequest,
  PedidoResponse,
  ProductoRequest
} from '../models/api.model';

@Service()
export class RestauranteApiService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/v1';

  obtenerProductos(): Observable<ProductoApi[]> {
    return this.http.get<ProductoApi[]>(
      `${this.apiUrl}/productos`
    );
  }
  crearProducto(producto: ProductoRequest): Observable<ProductoApi> {
  return this.http.post<ProductoApi>(
    `${this.apiUrl}/productos`,
    producto
  );
}

  obtenerCategorias(): Observable<CategoriaApi[]> {
    return this.http.get<CategoriaApi[]>(
      `${this.apiUrl}/categorias`
    );
  }

  obtenerMesas(): Observable<MesaApi[]> {
    return this.http.get<MesaApi[]>(
      `${this.apiUrl}/mesas`
    );
  }

  crearMesa(mesa: MesaRequest): Observable<MesaApi> {
    return this.http.post<MesaApi>(
      `${this.apiUrl}/mesas`,
      mesa
    );
  }

  obtenerClientes(): Observable<ClienteApi[]> {
    return this.http.get<ClienteApi[]>(
      `${this.apiUrl}/clientes`
    );
  }

  crearCliente(cliente: ClienteRequest): Observable<ClienteApi> {
    return this.http.post<ClienteApi>(
      `${this.apiUrl}/clientes`,
      cliente
    );
  }

  obtenerPedidos(): Observable<PedidoResponse[]> {
    return this.http.get<PedidoResponse[]>(
      `${this.apiUrl}/pedidos`
    );
  }

  crearPedido(pedido: PedidoRequest): Observable<PedidoResponse> {
    return this.http.post<PedidoResponse>(
      `${this.apiUrl}/pedidos`,
      pedido
    );
  }
}
