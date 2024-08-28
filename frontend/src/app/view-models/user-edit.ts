export class UserEdit {
  email: string = "";
  gender: string = "";
  dateOfBirth: Date = new Date();
  fullname: string = "";
  role: "member" | "admin" = "member";
}
