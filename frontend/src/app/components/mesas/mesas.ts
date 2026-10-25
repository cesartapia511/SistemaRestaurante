import { Component, computed, inject } from '@angular/core';
import { RestauranteService } from '../../services/restaurante.service';
import { EstadoMesaDirective } from '../../directives/estado-mesa';

@Component({
  imports: [EstadoMesaDirective],
  selector: 'app-mesas',
  styleUrl: './mesas.css',
  templateUrl: './mesas.html',
})
export class Mesas {
  private readonly api = inject(RestauranteService);
  readonly mesas = this.api.mesas;

  readonly libres = computed(() => this.mesas().filter((m) => m.estado === 'libre').length);
  readonly ocupadas = computed(() => this.mesas().filter((m) => m.estado === 'ocupada').length);
  readonly reservadas = computed(() => this.mesas().filter((m) => m.estado === 'reservada').length);
}