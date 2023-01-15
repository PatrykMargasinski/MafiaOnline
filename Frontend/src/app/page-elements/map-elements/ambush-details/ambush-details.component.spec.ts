import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmbushDetailsComponent } from './ambush-details.component';

describe('AmbushDetailsComponent', () => {
  let component: AmbushDetailsComponent;
  let fixture: ComponentFixture<AmbushDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AmbushDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AmbushDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
