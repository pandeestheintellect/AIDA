import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityShareholdersComponent } from './entity-shareholders.component';

describe('EntityShareholdersComponent', () => {
  let component: EntityShareholdersComponent;
  let fixture: ComponentFixture<EntityShareholdersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityShareholdersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityShareholdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
