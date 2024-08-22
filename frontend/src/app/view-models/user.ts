export class User {
  fullName: string = 'Nguyen Van A';
  role: 'librarian' | 'member' | string = 'member';
  email: string = 'asd@gmail.com';
  gender: string = 'male';
  dateOfBirth: Date = new Date();
}
