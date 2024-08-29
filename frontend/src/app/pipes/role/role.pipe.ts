import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'role',
  standalone: true
})
export class RolePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (value instanceof String || typeof value === 'string') {
      switch (value) {
        case 'member':
          return 'Thành viên';
        case 'admin':
          return 'Thủ thư';
        default:
          return value;
      }
    }
    return value;
  }

}
