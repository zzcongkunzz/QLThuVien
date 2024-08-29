import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'dateTime',
  standalone: true
})
export class DateTimePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (value instanceof Date) {
      return `${value.toLocaleDateString('vi-VN')}, ${value.toLocaleTimeString('vi-VN')}`;
    }
    return value;
  }

}
