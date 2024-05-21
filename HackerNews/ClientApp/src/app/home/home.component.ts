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
  dataSource: any;
  pageSize = 10;
  pageIndex = 0;
  stories: any[] = [];
  totalElements = 0;
  searchTerm = '';
  pageNumber = 10;

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

  search(event: KeyboardEvent) {
  //  this.get((event.target as HTMLTextAreaElement).value);
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

