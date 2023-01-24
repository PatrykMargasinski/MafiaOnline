import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotActivatedPlayerComponent } from './not-activated-player.component';

describe('NotActivatedPlayerComponent', () => {
  let component: NotActivatedPlayerComponent;
  let fixture: ComponentFixture<NotActivatedPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotActivatedPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotActivatedPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
