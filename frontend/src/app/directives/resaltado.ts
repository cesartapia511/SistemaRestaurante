import { Directive, input } from '@angular/core';

@Directive({
  selector: '[appResaltado]',
  host: {
    '[style.background-color]': 'color()',
    '[style.transition]': '"background-color .2s ease"',
  },
})
export class ResaltadoDirective {
  color = input<string>('#fde68a');
}
