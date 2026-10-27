
import { Component, computed, inject, signal } from '@angular/core';
import { finalize, switchMap, throwError } from 'rxjs';

import { RestauranteService } from '../../../services/restaurante.service';
import { RestauranteApiService } from '../../../services/restaurante-api.service';
import { MonedaBsPipe } from '../../../pipes/moneda-bs-pipe';

@Component({
  imports: [MonedaBsPipe],
  selector: 'app-menu-list',
  styleUrl: './menu-list.css',
  templateUrl: './menu-list.html'
})
export class MenuList {
  private readonly api = inject(RestauranteService);
  private readonly apiHttp = inject(RestauranteApiService);

  readonly busqueda = signal('');
  readonly categoriaSeleccionada = signal<number | null>(null);

  readonly categorias = this.api.categorias;

  readonly nuevoNombre = signal('');
  readonly nuevaDescripcion = signal('');
  readonly nuevaImagenUrl = signal('');
  readonly nuevoPrecio = signal<number>(1);
  readonly nuevaCategoriaId = signal<number | null>(null);

  readonly guardandoProducto = signal(false);
  readonly mensajeProducto = signal('');
  readonly errorProducto = signal('');

  readonly platillosFiltrados = computed(() => {
    const q = this.busqueda().toLowerCase().trim();
    const cat = this.categoriaSeleccionada();

    return this.api.platillos().filter((p) => {
      const okCat = cat === null || p.categoriaId === cat;
      const okQ = q === '' || p.nombre.toLowerCase().includes(q);
      return okCat && okQ && p.disponible;
    });
  });

  onBusqueda(e: Event): void {
    this.busqueda.set(
      (e.target as HTMLInputElement).value
    );
  }

  onNuevoNombre(event: Event): void {
    this.nuevoNombre.set(
      (event.target as HTMLInputElement).value
    );
  }

  onNuevaDescripcion(event: Event): void {
    this.nuevaDescripcion.set(
      (event.target as HTMLTextAreaElement).value
    );
  }

  onNuevaImagenUrl(event: Event): void {
    this.nuevaImagenUrl.set(
      (event.target as HTMLInputElement).value
    );
  }

  onNuevoPrecio(event: Event): void {
    this.nuevoPrecio.set(
      Number((event.target as HTMLInputElement).value)
    );
  }

  onNuevaCategoria(event: Event): void {
    const valor = (event.target as HTMLSelectElement).value;

    this.nuevaCategoriaId.set(
      valor === '' ? null : Number(valor)
    );
  }

  registrarProducto(): void {
    if (this.guardandoProducto()) {
      return;
    }

    this.mensajeProducto.set('');
    this.errorProducto.set('');

    const nombre = this.nuevoNombre().trim();
    const descripcion = this.nuevaDescripcion().trim();
    const imagenUrl = this.nuevaImagenUrl().trim();
    const precio = this.nuevoPrecio();
    const categoriaId = this.nuevaCategoriaId();

    if (!nombre || nombre.length > 50) {
      this.errorProducto.set(
        'El nombre es obligatorio y debe tener máximo 50 caracteres.'
      );
      return;
    }

    if (!Number.isFinite(precio) || precio < 1 || precio > 1000) {
      this.errorProducto.set(
        'El precio debe estar entre Bs 1 y Bs 1000.'
      );
      return;
    }

    if (
      categoriaId === null ||
      !this.categorias().some((c) => c.id === categoriaId)
    ) {
      this.errorProducto.set(
        'Selecciona una categoría válida.'
      );
      return;
    }

    if (imagenUrl) {
      try {
        const url = new URL(imagenUrl);

        if (!['http:', 'https:'].includes(url.protocol)) {
          this.errorProducto.set(
            'La imagen debe tener una URL HTTP o HTTPS válida.'
          );
          return;
        }
      } catch {
        this.errorProducto.set(
          'Ingresa una URL de imagen válida.'
        );
        return;
      }
    }

    this.guardandoProducto.set(true);

    this.apiHttp.crearProducto({
      nombre,
      descripcion: descripcion || null,
      imagenUrl: imagenUrl || null,
      precio,
      categoriaId
    })
      .pipe(
        switchMap((producto) => {
          if (!producto) {
            return throwError(
              () => new Error('No se pudo registrar el platillo.')
            );
          }

          return this.api.cargarPlatillos();
        }),
        finalize(() => this.guardandoProducto.set(false))
      )
      .subscribe({
        next: () => {
          this.busqueda.set('');
          this.categoriaSeleccionada.set(null);

          this.mensajeProducto.set(
            `Platillo ${nombre} registrado correctamente.`
          );

          this.nuevoNombre.set('');
          this.nuevaDescripcion.set('');
          this.nuevaImagenUrl.set('');
          this.nuevoPrecio.set(1);
          this.nuevaCategoriaId.set(null);
        },
        error: (error) => {
          console.error(
            'Error al registrar platillo:',
            error
          );

          this.errorProducto.set(
            error instanceof Error && !('status' in error)
              ? error.message
              : 'No se pudo completar el registro. Revisa el backend y los datos enviados.'
          );
        }
      });
  }
}
