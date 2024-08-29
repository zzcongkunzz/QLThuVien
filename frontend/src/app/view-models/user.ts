export class User {
  id: string = "123"
  fullName: string = 'Nguyen Van A';
  role: 'admin' | 'member' = 'member';
  email: string = 'asd@gmail.com';
  gender: string = 'male';
  dateOfBirth: Date = new Date();
}
