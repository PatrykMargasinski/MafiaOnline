import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HowToPlayMapComponent } from './how-to-play-map.component';

describe('HowToPlayMapComponent', () => {
  let component: HowToPlayMapComponent;
  let fixture: ComponentFixture<HowToPlayMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HowToPlayMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HowToPlayMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
