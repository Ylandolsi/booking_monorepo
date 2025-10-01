import type { Item } from '@/components';
import { create } from 'zustand';

type SideBarState = {
  sidebarOpen: boolean;
  itemActive: Item['name'];
  setItemActive: (itemName: Item['name']) => void;
  setSidebarOpen: (open?: boolean) => void;
  toggleSidebar: () => void;
};

export const useSideBar = create<SideBarState>((set) => ({
  sidebarOpen: false,
  itemActive: 'Home',
  setItemActive: (itemName: Item['name']) => set({ itemActive: itemName }),
  setSidebarOpen: (open) => set({ sidebarOpen: open }),
  toggleSidebar: () => set((state) => ({ sidebarOpen: !state.sidebarOpen })),
}));
