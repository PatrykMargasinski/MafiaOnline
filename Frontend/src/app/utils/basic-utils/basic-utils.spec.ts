import { TestBed } from '@angular/core/testing';

import { BasicUtils } from './basic-utils';

describe('BasicUtilsService', () => {
  let service: BasicUtils;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BasicUtils);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
