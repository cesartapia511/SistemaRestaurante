
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ProductoApi,
  CategoriaApi,
  MesaApi,
  ClienteApi,
  PedidoRequest,
  PedidoResponse
} from '../models/api.model';

@Injectable({
  providedIn: 'root'
})
export class RestauranteApiService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/v1';

  obtenerProductos(): Observable<ProductoApi[]> {
    return this.http.get<ProductoApi[]>(
      `${this.apiUrl}/productos`
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

  obtenerClientes(): Observable<ClienteApi[]> {
    return this.http.get<ClienteApi[]>(
      `${this.apiUrl}/clientes`
    );
  }

  obtenerPedidos(): Observable<PedidoResponse[]> {
    return this.http.get<PedidoResponse[]>(
      `${this.apiUrl}/pedidos`
    );
  }

  crearPedido(
    pedido: PedidoRequest
  ): Observable<PedidoResponse> {
    return this.http.post<PedidoResponse>(
      `${this.apiUrl}/pedidos`,
      pedido
    );
  }
}
