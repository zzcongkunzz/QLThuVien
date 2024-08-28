export class UserCreate {
  email: string = "";
  password: string = "";
  gender: string = "";
  dateOfBirth: Date = new Date();
  fullname: string = "";
  role: "member" | "admin" = "member";
}
