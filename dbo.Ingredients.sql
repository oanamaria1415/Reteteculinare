CREATE TABLE [dbo].[Ingredients] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NOT NULL,
    [Quantity] FLOAT (53)     NOT NULL,
    [Unit]     NVARCHAR (MAX) NOT NULL,
    [RecipeID] INT            NOT NULL,
    CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Ingredients_Recipe_RecipeID] FOREIGN KEY ([RecipeID]) REFERENCES [dbo].[Recipe] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Ingredients_RecipeID]
    ON [dbo].[Ingredients]([RecipeID] ASC);





