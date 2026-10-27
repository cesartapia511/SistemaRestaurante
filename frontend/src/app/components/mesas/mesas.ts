
import { Component, computed, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { RestauranteService } from '../../services/restaurante.service';
import { EstadoMesaDirective } from '../../directives/estado-mesa';

@Component({
  imports: [EstadoMesaDirective],
  selector: 'app-mesas',
  styleUrl: './mesas.css',
  templateUrl: './mesas.html'
})
export class Mesas {
  private readonly api = inject(RestauranteService);

  readonly mesas = this.api.mesas;

  readonly numero = signal<number>(2);
  readonly capacidad = signal<number>(4);
  readonly guardando = signal(false);
  readonly mensaje = signal('');
  readonly error = signal('');

  readonly libres = computed(() =>
    this.mesas().filter(
      (mesa) =>
        ['libre', 'disponible'].includes(
          mesa.estado.toLowerCase()
        )
    ).length
  );

  readonly ocupadas = computed(() =>
    this.mesas().filter(
      (mesa) =>
        ['ocupada', 'ocupado'].includes(
          mesa.estado.toLowerCase()
        )
    ).length
  );

  readonly reservadas = computed(() =>
    this.mesas().filter(
      (mesa) =>
        ['reservada', 'reservado'].includes(
          mesa.estado.toLowerCase()
        )
    ).length
  );

  cambiarNumero(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.numero.set(Number(input.value));
  }

  cambiarCapacidad(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.capacidad.set(Number(input.value));
  }

  crearMesa(): void {
    if (this.guardando()) {
      return;
    }

    this.mensaje.set('');
    this.error.set('');

    const numero = this.numero();
    const capacidad = this.capacidad();

    if (
      !Number.isInteger(numero) ||
      numero < 1 ||
      numero > 200
    ) {
      this.error.set(
        'El número de mesa debe estar entre 1 y 200.'
      );
      return;
    }

    if (
      !Number.isInteger(capacidad) ||
      capacidad < 2 ||
      capacidad > 12
    ) {
      this.error.set(
        'La capacidad debe estar entre 2 y 12 personas.'
      );
      return;
    }

    if (this.mesas().some((mesa) => mesa.numero === numero)) {
      this.error.set('Ya existe una mesa con ese número.');
      return;
    }

    this.guardando.set(true);

    this.api.crearMesa({ numero, capacidad })
      .pipe(
        finalize(() => this.guardando.set(false))
      )
      .subscribe({
        next: (mesa) => {
          if (!mesa) {
            this.error.set('No se pudo registrar la mesa.');
            this.api.cargarMesas();
            return;
          }

          this.mensaje.set(
            `Mesa ${mesa.numero} registrada correctamente.`
          );

          this.numero.set(mesa.numero + 1);
          this.capacidad.set(4);
        },
        error: (error) => {
          console.error('Error al registrar mesa:', error);

          this.error.set(
            'No se pudo registrar la mesa. Revisa la conexión o el número ingresado.'
          );
        }
      });
  }
}
