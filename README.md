FactoryPunchList was made as a proof of concept to manage BIM 360 punchlist through planted QR Codes in the factory. 

This Repository uses an Azure SQL Database in place of BIM 360 to show all CRUD functions that can happen when an employee accesses through the QR code.

The main goal was to use ASP.NET to serve as a UI that would pull punchlist data from BIM 360 through the Forge API when it was called by the QR code.
The user could then make changes, add images, and then submit an update to the punch which FactoryPunchList would handle and post again through the Forge API.
