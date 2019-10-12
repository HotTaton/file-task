import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileProcessorComponent } from './file-processor.component';

describe('FileProcessorComponent', () => {
  let component: FileProcessorComponent;
  let fixture: ComponentFixture<FileProcessorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileProcessorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileProcessorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
