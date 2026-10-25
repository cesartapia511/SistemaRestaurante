import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'monedaBs' })
export class MonedaBsPipe implements PipeTransform {
  transform(valor: number | null | undefined): string {
    if (valor == null) return 'Bs 0.00';
    return `Bs ${valor.toFixed(2)}`;
  }
}