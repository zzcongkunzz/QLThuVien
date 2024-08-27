import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EntryBookComponent } from './entry-book.component';

describe('EntryBookComponent', () => {
  let component: EntryBookComponent;
  let fixture: ComponentFixture<EntryBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EntryBookComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntryBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
