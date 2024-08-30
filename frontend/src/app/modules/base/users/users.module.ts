import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {UsersManagePageComponent} from "./pages/users-manage-page/users-manage-page.component";
import {UserEditPageComponent} from "./pages/user-edit-page/user-edit-page.component";
import {UserProfilePageComponent} from "./pages/user-profile-page/user-profile-page.component";

const routes: Routes = [
  {
    path: 'manage',
    component: UsersManagePageComponent
  },
  {
    path: 'edit/:id',
    component: UserEditPageComponent
  },
  {
    path: 'profile',
    component: UserProfilePageComponent
  },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule.forChild(routes)
  ],
  exports: [RouterModule],
})
export class UsersModule {
}
