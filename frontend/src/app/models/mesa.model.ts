export type EstadoMesa = 'libre' | 'ocupada' | 'reservada';

export interface Mesa {
  id: number;
  numero: number;
  capacidad: number;
  estado: EstadoMesa;
}