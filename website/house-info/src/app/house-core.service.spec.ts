import { TestBed } from '@angular/core/testing';

import { HouseCoreService } from './house-core.service';

describe('HouseCoreService', () => {
  let service: HouseCoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HouseCoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
