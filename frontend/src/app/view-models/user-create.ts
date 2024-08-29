export class UserCreate {
  email: string = "";
  password: string = "";
  gender: string = "";
  dateOfBirth: Date = new Date();
  fullName: string = "";
  role: "member" | "admin" = "member";
}
