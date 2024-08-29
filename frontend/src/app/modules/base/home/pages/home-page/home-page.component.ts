import {Component, Inject, OnInit} from '@angular/core';
import {AuthService} from "../../../../../services/auth/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  providers: [{ provide: 'AUTH_SERVICE_INJECTOR', useClass: AuthService }],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit{
  private localStorage: Storage | undefined;

  constructor(
    @Inject('AUTH_SERVICE_INJECTOR') private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
     if(!this.authService.isAuthenticated()){
       this.router.navigate(['auth/login']);
     }
  }


}
