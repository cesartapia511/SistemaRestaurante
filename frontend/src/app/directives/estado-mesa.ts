import { computed, Directive, input } from '@angular/core';
import { EstadoMesa } from '../models/mesa.model';

@Directive({
  selector: '[appEstadoMesa]',
  host: {
    '[style.background-color]': 'color()',
  },
})
export class EstadoMesaDirective {
  estado = input.required<EstadoMesa>();

  protected readonly color = computed(() => {
    const colores: Record<EstadoMesa, string> = {
      libre: '#d1fae5',
      ocupada: '#fee2e2',
      reservada: '#fef3c7',
    };
    return colores[this.estado()];
  });
}
