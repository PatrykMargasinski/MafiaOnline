import { TestBed } from '@angular/core/testing';

import { AmbushService } from './ambush.service';

describe('AmbushService', () => {
  let service: AmbushService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AmbushService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
