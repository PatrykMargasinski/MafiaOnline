import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HowToPlayIntroductionComponent } from './how-to-play-introduction.component';

describe('HowToPlayIntroductionComponent', () => {
  let component: HowToPlayIntroductionComponent;
  let fixture: ComponentFixture<HowToPlayIntroductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HowToPlayIntroductionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HowToPlayIntroductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
