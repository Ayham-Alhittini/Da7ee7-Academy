export interface SectionItem {
    id: number;
    sectionItemTitle: string;
    orderNumber: number;
    type: number;//(1 file) (2 video)
    contentUrl: string;
    videoLength: number;
    fileId: string;
    watchedDate: Date;//only if it's a lecture
}