import {Pipe, PipeTransform} from '@angular/core';


@Pipe({
  name: 'gender',
  standalone: true
})
export class GenderPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (value instanceof String || typeof value === 'string') {
      switch (value) {
        case 'male':
          return 'Nam';
        case 'female':
          return 'Nữ';
        default:
          return value;
      }
    }
    return value;
  }

}
