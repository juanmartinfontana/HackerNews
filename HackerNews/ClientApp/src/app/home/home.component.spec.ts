import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { NewsStoryService } from '../services/news-story.service';
import { of } from 'rxjs';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let newsStoryService: jasmine.SpyObj<NewsStoryService>;

  beforeEach(async () => {
    const newsStoryServiceSpy = jasmine.createSpyObj('NewsStoryService', ['getAllStories']);

    await TestBed.configureTestingModule({
      declarations: [HomeComponent],
      providers: [
        { provide: NewsStoryService, useValue: newsStoryServiceSpy }
      ]
    }).compileComponents();

    newsStoryService = TestBed.inject(NewsStoryService) as jasmine.SpyObj<NewsStoryService>;
    newsStoryService.getAllStories.and.returnValue(of());
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadStories on init', () => {
    const loadStoriesSpy = spyOn(component, 'loadStories');
    component.ngOnInit();
    expect(loadStoriesSpy).toHaveBeenCalled();
  });

  it('should unsubscribe on destroy', () => {
    const unsubscribeSpy = spyOn(component.subscription, 'unsubscribe');
    component.ngOnDestroy();
    expect(unsubscribeSpy).toHaveBeenCalled();
  });

  it('should load stories', () => {
    const testData = {
      items: [{ id: 1, title: 'Story 1' }, { id: 2, title: 'Story 2' }],
      totalCount: 2
    };
    newsStoryService.getAllStories.and.returnValue(of(testData));

    component.loadStories();

    expect(newsStoryService.getAllStories).toHaveBeenCalledWith(component.searchTerm, component.pageNumber, component.pageSize);
    expect(component.stories).toEqual(testData.items);
    expect(component.totalElements).toEqual(testData.totalCount);
  });

  it('should fetch stories on search', () => {
    const mockData = {
      items: [{ title: 'Story 1' }, { title: 'Story 2' }],
      totalCount: 2
    };

    newsStoryService.getAllStories.and.returnValue(of(mockData));

    component.search('searchValue');

    expect(newsStoryService.getAllStories).toHaveBeenCalledWith('searchValue', component.pageNumber, component.pageSize);
    expect(component.stories).toEqual(mockData.items);
    expect(component.totalElements).toEqual(mockData.totalCount);
  });
});
