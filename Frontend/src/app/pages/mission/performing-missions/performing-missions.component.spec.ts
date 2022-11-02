import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformingMissionsComponent } from './performing-missions.component';

describe('PerformingMissionsComponent', () => {
  let component: PerformingMissionsComponent;
  let fixture: ComponentFixture<PerformingMissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformingMissionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformingMissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
