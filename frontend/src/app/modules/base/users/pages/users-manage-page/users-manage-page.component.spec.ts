import {ComponentFixture, TestBed} from '@angular/core/testing';

import {UsersManagePageComponent} from './users-manage-page.component';

describe('UserManagePageComponent', () => {
  let component: UsersManagePageComponent;
  let fixture: ComponentFixture<UsersManagePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersManagePageComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(UsersManagePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
