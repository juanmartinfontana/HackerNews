import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { NewsStoryService } from './news-story.service';
import { HackerNewsStory } from '../Models/hackerNewsStory.model';

const mockData: HackerNewsStory[] =
  [
    {
      "title": "Statement from Scarlett Johansson on the OpenAI \"Sky\" voice",
      "by": "mjcl",
      "url": "https://twitter.com/BobbyAllyn/status/1792679435701014908"
    },
    {
      "title": "OpenAI departures: Why can’t former employees talk?",
      "by": "fnbr",
      "url": "https://www.vox.com/future-perfect/2024/5/17/24158478/openai-departures-sam-altman-employees-chatgpt-release"
    },
  ];

describe('NewsStoryService', () => {
  let service: NewsStoryService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        NewsStoryService,
        { provide: 'BASE_URL', useValue: 'https://example.com/api/' }
      ]
    });
    service = TestBed.inject(NewsStoryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getAllStories with correct parameters', () => {
    const pageNumber = 1;
    const pageSize = 10;

    service.getAllStories('', pageNumber, pageSize).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const request = httpMock.expectOne(`https://example.com/api/weatherforecast?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    expect(request.request.method).toBe('GET');
    request.flush(mockData);
  });

});
