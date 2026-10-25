import { Component, computed, inject } from '@angular/core';
import { RestauranteService } from '../../services/restaurante.service';
import { MonedaBsPipe } from '../../pipes/moneda-bs-pipe';

@Component({
  imports: [MonedaBsPipe],
  selector: 'app-dashboard',
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})
export class Dashboard {
  private readonly api = inject(RestauranteService);

  readonly pedidos = this.api.pedidos;
  readonly mesas = this.api.mesas;
  readonly platillos = this.api.platillos;

  readonly totalVentas = computed(() =>
    this.pedidos()
      .filter((p) => p.estado !== 'cancelado')
      .reduce((acc, p) => acc + this.api.totalPedido(p), 0)
  );

  readonly pedidosHoy = computed(() => this.pedidos().length);
  readonly mesasOcupadas = computed(() => this.mesas().filter((m) => m.estado === 'ocupada').length);
  readonly platillosDisponibles = computed(() => this.platillos().filter((p) => p.disponible).length);
}
