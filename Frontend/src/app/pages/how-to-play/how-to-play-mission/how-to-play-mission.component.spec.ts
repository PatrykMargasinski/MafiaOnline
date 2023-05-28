import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HowToPlayMissionComponent } from './how-to-play-mission.component';

describe('HowToPlayMissionComponent', () => {
  let component: HowToPlayMissionComponent;
  let fixture: ComponentFixture<HowToPlayMissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HowToPlayMissionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HowToPlayMissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
