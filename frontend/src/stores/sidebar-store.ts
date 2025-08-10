import type { Item } from '@/components';
import { create } from 'zustand';

type SideBarState = {
  sidebarOpen: boolean;
  itemActive: Item['name'];
  collapsed?: boolean;
  setCollapsed?: (collapsed: boolean) => void;
  setItemActive: (itemName: Item['name']) => void;
  setSidebarOpen: (open?: boolean) => void;
};

export const useSideBar = create<SideBarState>((set) => ({
  sidebarOpen: false,
  itemActive: 'Home',
  collapsed: false,
  setCollapsed: (collapsed: boolean) => set({ collapsed: collapsed }),
  setItemActive: (itemName: Item['name']) => set({ itemActive: itemName }),
  setSidebarOpen: (open) => set({ sidebarOpen: open }),
}));
