import { SectionItem } from "./sectionItem";

export interface Section {
    id: number;
    sectionTitle: string;
    totalSectionTime: number;
    orderNumber: number;
    courseId: number;
    sectionItems: SectionItem[];
}