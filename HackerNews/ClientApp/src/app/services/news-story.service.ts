import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { HackerNewsStory } from '../Models/hackerNewsStory.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsStoryService {

  constructor(private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string) {}

  getAllStories(searchTerm: string, pageNumber?: number, pageSize?: number): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'weatherforecast' + `?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
