import { Component, computed, inject, signal } from '@angular/core';
import { RestauranteService } from '../../../services/restaurante.service';
import { MonedaBsPipe } from '../../../pipes/moneda-bs-pipe';

@Component({
  imports: [MonedaBsPipe],
  selector: 'app-menu-list',
  styleUrl: './menu-list.css',
  templateUrl: './menu-list.html',
})
export class MenuList {
  private readonly api = inject(RestauranteService);

  readonly busqueda = signal('');
  readonly categoriaSeleccionada = signal<number | null>(null);

  readonly categorias = this.api.categorias;
  readonly platillosFiltrados = computed(() => {
    const q = this.busqueda().toLowerCase().trim();
    const cat = this.categoriaSeleccionada();
    return this.api.platillos().filter((p) => {
      const okCat = cat === null || p.categoriaId === cat;
      const okQ = q === '' || p.nombre.toLowerCase().includes(q);
      return okCat && okQ && p.disponible;
    });
  });

  onBusqueda(e: Event) {
    this.busqueda.set((e.target as HTMLInputElement).value);
  }
}
