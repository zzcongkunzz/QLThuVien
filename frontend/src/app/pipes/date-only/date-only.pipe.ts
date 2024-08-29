import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'dateOnly',
  standalone: true
})
export class DateOnlyPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (value instanceof Date) {
      return `${value.getDate()}-${value.getMonth() + 1 > 10 ? value.getMonth() + 1 : '0' + (value.getMonth() + 1)}-${value.getFullYear()}`;
    }
    return value;
  }

}
