import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { NewsStoryService } from '../services/news-story.service';
import { HackerNewsStory } from '../Models/hackerNewsStory.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  public hackerNewsStories: HackerNewsStory[] | undefined;
  public subscription: Subscription = new Subscription();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator | undefined;
  public pageSize = 10;
  public stories: any[] = [];
  public totalElements = 0;
  public searchTerm = '';
  public pageNumber = 10;

  constructor(
    private newsStoryServices: NewsStoryService) { }


  ngOnInit(): void {
    this.loadStories();
  }

  loadStories(event?: any) {
    if (event) {
      this.pageNumber = event.pageIndex + 1;
    }

    this.subscription = this.newsStoryServices.getAllStories(this.searchTerm, this.pageNumber, this.pageSize)
      .subscribe(data => {
        this.stories = data.items;
        this.totalElements = data.totalCount;
      });
  }

  search(searchValue: any) {
    this.subscription = this.newsStoryServices.getAllStories(searchValue, this.pageNumber, this.pageSize)
      .subscribe(data => {
        this.stories = data.items;
        this.totalElements = data.totalCount;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  open(url: string) {
    window.open(url, "_blank");
  }
}

