export interface Categoria {
  id: number;
  nombre: string;
}

export interface Platillo {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  categoriaId: number;
  disponible: boolean;
}