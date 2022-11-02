import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableMissionsComponent } from './available-missions.component';

describe('AvailableMissionsComponent', () => {
  let component: AvailableMissionsComponent;
  let fixture: ComponentFixture<AvailableMissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvailableMissionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AvailableMissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
